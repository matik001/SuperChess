import { create, StateCreator } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';

export const createStoreUtil = <State>(
	storeName: string,
	createState: StateCreator<
		State,
		[['zustand/immer', never], ['zustand/devtools', never]],
		[],
		State
	>
) => create<State>()(immer(devtools(createState, { name: storeName, store: storeName })));
