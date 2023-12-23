import { createStoreUtil } from './storeUtils';

export interface AppSettings {
	version: string;
}

export interface AppSettingsState {
	data: AppSettings;
	isLoading: boolean;
	isFetching: boolean;
	error?: Error;
}
export interface AppSettingsActions {
	fetch: () => Promise<void>;
}

const useAppSettingsStore = createStoreUtil<AppSettingsState & AppSettingsActions>(
	'AppSettings',
	(set, get) => ({
		isLoading: true,
		isFetching: false,
		data: {
			version: ''
		},
		fetch: async () => {
			try {
				if (get().isFetching) return;
				set({ isLoading: true, isFetching: true });
				// const data = await fetchAppSettings();
				const data = { version: '1' }; /// TODO

				set({ isLoading: false, isFetching: false, data: data });
			} catch (error) {
				set({ isLoading: false, isFetching: false, error: error as Error });
			}
		}
	})
);

export default useAppSettingsStore;
