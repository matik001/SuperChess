import * as signalR from '@microsoft/signalr';
import { HubConnectionState } from '@microsoft/signalr';
import { GameDTO, GameType } from 'api/gameApi';
import { PieceSymbol } from 'chess.js';
import i18next from 'i18next';
import { atom, useAtom } from 'jotai';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import useAuthStore from 'store/authStore';

const connection = new signalR.HubConnectionBuilder()
	.withUrl('/hub/games', {
		accessTokenFactory: () => useAuthStore.getState().tokens?.token ?? ''
	})
	.withAutomaticReconnect()
	.build();

// neccessary line for enabling duplex communication (it must execute be before start function)
connection.on('anything', () => {});

const useReceiveObj = <T>(methodName: string, onReceive: (obj: T) => void) => {
	useEffect(() => {
		const onReceivedObj = (data: T) => {
			onReceive(data);
		};
		connection.on(methodName, onReceivedObj);
		return () => {
			connection.off(methodName, onReceivedObj);
		};
	}, [methodName, onReceive]);
};
const useObject = <T>(methodName: string, filter?: (obj: T) => boolean) => {
	const [obj, setObj] = useState<T>();
	const onReceive = useCallback(
		(obj: T) => {
			if (filter && !filter(obj)) return;
			setObj(obj);
		},
		[filter]
	);
	useReceiveObj(methodName, onReceive);

	return [obj, setObj] as [typeof obj, typeof setObj];
};
const useGame = (gameGuid: string) => {
	return useObject<GameDTO>('UpdateGame', (g) => g.gameGuid === gameGuid);
};
const useGames = () => {
	const [games, setGames] = useObject<GameDTO[]>('UpdateGamesInRoom');
	useReceiveObj<GameDTO>('UpdateGame', (game) => {
		if (games && game) {
			const newGames = games.map((a) => (a.gameGuid === game.gameGuid ? game : a));
			setGames(newGames);
		}
	});
	return [games, setGames] as [typeof games, typeof setGames];
};

export interface Position {
	x: number;
	y: number;
}
export type PieceType = 'None' | 'Pawn' | 'Knight' | 'Bishop' | 'Rook' | 'Queen' | 'King';
export const positionFromString = (chessPos: string) => {
	/// a8->0,7
	return {
		x: chessPos.charCodeAt(0) - 'a'.charCodeAt(0),
		y: parseInt(chessPos.charAt(1)) - 1
	} as Position;
};
export const pieceSymbolToType = (symbol: PieceSymbol) => {
	return (
		{
			b: 'Bishop',
			p: 'Pawn',
			n: 'Knight',
			k: 'King',
			r: 'Rook',
			q: 'Queen'
		} as Record<PieceSymbol, PieceType>
	)[symbol];
};
export interface Move {
	from: Position;
	to: Position;
	promotion?: PieceType;
}
/// TODO someone else can send our move when we are guest - security issue
const makeMove = async (guestGuid: string | undefined, gameGuid: string, move: Move) => {
	await connection.send('MakeMove', guestGuid, gameGuid, move);
};
const joinGame = async (guestGuid: string | undefined, nick: string, gameGuid: string) => {
	await connection.send('JoinGame', guestGuid, nick, gameGuid);
};

// returns new game's guid
const createGame = async (
	guestGuid: string | undefined,
	nick: string,
	roomId: number,
	gameType: GameType
) => {
	return await connection.invoke<string>('CreateGame', guestGuid, nick, roomId, gameType);
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
		useGame,
		createGame,
		joinGame,
		makeMove
	};
};
