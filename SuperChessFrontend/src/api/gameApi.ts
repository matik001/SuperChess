import { appAxios } from './apiConfig';
import { UserDTO } from './userApi';

export type GameState =
	| 'NotStarted'
	| 'InProgress'
	| 'WaitingForMissingPlayer'
	| 'Finished'
	| 'Aborted';

export type GameType = 'Chess';

export type ChessGameResult = 'WinWhite' | 'WinBlack' | 'Draw';

export type PlayerColors = 'White' | 'Black';
/// If you add more games, you can add more colors here
interface ChessData {
	positionFEN: string;
	result?: ChessGameResult;
}
interface UserGameDTO {
	id: number;
	color: PlayerColors;
	nick: string;
	user?: UserDTO;
	gameId: number;
	guestGuid?: string;
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
export const QUERYKEY_GETGAMES = 'QUERYKEY_GETGAMES';
export const getGames = async (signal?: AbortSignal) => {
	const res = await appAxios.get<GameDTO[]>('/v1/Game', {
		signal: signal
	});
	return res.data;
};
