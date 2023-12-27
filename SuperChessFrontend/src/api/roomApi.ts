import { appAxios } from './apiConfig';

export interface RoomDTO {
	id: number;
	roomName: string;
	creationDate: Date;
}
export type RoomCreateDTO = Omit<RoomDTO, 'id' | 'creationDate'>;

export interface RoomExtendedDTO extends RoomDTO {
	amountOfUsers: number;
}

export const QUERYKEY_GETROOMS = 'QUERYKEY_GETROOMS';
export const getRooms = async (signal?: AbortSignal) => {
	const res = await appAxios.get<RoomExtendedDTO[]>('/v1/Room', {
		signal: signal
	});
	return res.data;
};

export const QUERYKEY_CREATEROOM = 'QUERYKEY_CREATEROOM';
export const createRoom = async (newRoom: RoomCreateDTO) => {
	const res = await appAxios.post<RoomDTO>('/v1/Room', newRoom);
	return res.data;
};

export const QUERYKEY_DELETEROOM = 'QUERYKEY_DELETEROOM';
export const deleteRooms = async (roomId: number) => {
	await appAxios.delete(`/v1/Room/${roomId}`);
};
