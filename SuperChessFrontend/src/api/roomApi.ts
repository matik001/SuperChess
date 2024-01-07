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
export const roomKeys = {
	prefix: ['room'] as const,
	list: () => [...roomKeys.prefix, 'list'] as const,
	one: (id: number) => [...roomKeys.prefix, 'one', id] as const
};

export const getRooms = async (signal?: AbortSignal) => {
	const res = await appAxios.get<RoomExtendedDTO[]>('/v1/Room', {
		signal: signal
	});
	return res.data;
};

export const createRoom = async (newRoom: RoomCreateDTO) => {
	const res = await appAxios.post<RoomDTO>('/v1/Room', newRoom);
	return res.data;
};

export const deleteRooms = async (roomId: number) => {
	await appAxios.delete(`/v1/Room/${roomId}`);
};
