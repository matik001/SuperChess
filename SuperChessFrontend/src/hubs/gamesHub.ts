import * as signalR from '@microsoft/signalr';
import { HubConnectionState } from '@microsoft/signalr';
import { UserDTO } from 'api/userApi';
import i18next from 'i18next';
import { atom, useAtom } from 'jotai';
import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import useAuthStore from 'store/authStore';

type GameState = 'NotStarted' | 'InProgress' | 'WaitingForMissingPlayer' | 'Finished' | 'Aborted';

type GameType = 'Chess';

type ChessGameResult = 'WinWhite' | 'WinBlack' | 'Draw';

type PlayerColors = 'White' | 'Black';
/// If you add more games, you can add more colors here
interface ChessData {
	positionPgn: string;
	result?: ChessGameResult;
}
interface UserGameDTO {
	id: number;
	color: PlayerColors;
	nick: string;
	user: UserDTO;
	gameId: number;
}
export interface GameDTO {
	gameGuid: string;
	creationDate: Date;
	gameStatus: GameState;
	gameType: GameType;
	chessData: ChessData;
	roomId: number;
	userGames: UserGameDTO[];
}
const connection = new signalR.HubConnectionBuilder()
	.withUrl('/hub/games', {
		accessTokenFactory: () => useAuthStore.getState().tokens?.token ?? ''
	})
	.withAutomaticReconnect()
	.build();

// neccessary line for enabling duplex communication (it must execute be before start function)
connection.on('anything', () => {});

const useSignalrObj = <T>(methodName: string) => {
	const [obj, setObj] = useState<T>();
	useEffect(() => {
		const onReceivedObj = (data: T) => {
			setObj(data);
		};
		connection.on(methodName, onReceivedObj);
		return () => {
			connection.off(methodName, onReceivedObj);
		};
	}, [methodName]);

	return obj;
};
const useGames = () => {
	return useSignalrObj<GameDTO[]>('UpdateGamesInRoom');
};
const createGame = async (roomId: number, gameType: GameType, nick: string) => {
	await connection.send('CreateGame', roomId, gameType, nick);
};
const joinRoom = async (roomId: number) => {
	await connection.send('JoinRoom', roomId);
};
const leaveRoom = async (roomId: number) => {
	await connection.send('LeaveRoom', roomId);
};

let lastToken: string | undefined = undefined;
const isConnectedAtom = atom(false);

/// call it whenever access tocken changes
export const useGamesHub = () => {
	const token = useAuthStore((a) => a.tokens?.token);

	const [isConnected, setIsConnected] = useAtom(isConnectedAtom);
	connection.onclose(() => setIsConnected(false));
	connection.onreconnected(() => setIsConnected(true));
	useEffect(() => {
		const init = async () => {
			if (connection.connectionId && lastToken === token) {
				setIsConnected(true);
				return;
			}
			if (connection.connectionId) {
				await connection.stop();
			}
			try {
				if (
					connection.state === HubConnectionState.Disconnecting ||
					connection.state === HubConnectionState.Disconnected
				) {
					await connection.start();
					lastToken = token;
					setIsConnected(true);
				}
			} catch (err) {
				setIsConnected(false);
				console.log(err);
				toast.error(i18next.t('Cannot connect to signalr hub'));
			}
		};
		init();
	}, [isConnected, setIsConnected, token]);
	return {
		isConnected: isConnected,
		joinRoom,
		leaveRoom,
		useGames,
		createGame
	};
};
