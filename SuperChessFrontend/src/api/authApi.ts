import { appAxios } from './apiConfig';

export type MessageRole = 'assistant' | 'system' | 'user';
export interface Message {
	id: number;
	role: MessageRole;
	content: string;
}
export interface Chat {
	id: number;
	title: string;
}
export interface ChatWithMessages extends Chat {
	messages?: Message[];
}

export const createChat_KEY = 'CreateChat';
export const createChat = async (title?: string) => {
	const res = await appAxios.post<unknown>('/chat', {
		...(title != null ? { title: title } : {})
	});
	return res.data;
};

export const getChats_KEY = 'GetChat';
export const getChats = async () => {
	const res = await appAxios.get<Chat[]>(`/chat`);
	return res.data;
};

export const getChatWithMessages_KEY = 'GetChatWithMessages';
export const getChatWithMessages = async (id: number) => {
	const res = await appAxios.get<ChatWithMessages>(`/chat/${id}`);
	return res.data;
};

export const deleteChat_KEY = 'DeleteChat';
export const deleteChat = async (id: number) => {
	const res = await appAxios.delete<ChatWithMessages>(`/chat/${id}`);
	return res.data;
};

export const createMessage_KEY = 'CreateMessage';
export const createMessage = async (chatId: number, content: string) => {
	const res = await appAxios.post<unknown>(`/chat/${chatId}/messages`, {
		content: content
	});
	return res.data;
};
