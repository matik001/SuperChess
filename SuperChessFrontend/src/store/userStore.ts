import { UserDTO } from 'api/userApi';
import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';
export interface UserStoreState {
	nick: string; /// if signin it is username
	user?: UserDTO; /// only if signed in
}
export interface UserStoreActions {
	setNickname: (nick: string) => void;
	setUser: (user: UserDTO | undefined) => void;
}

const StoreName = 'UserStore';
const useUserStore = create<UserStoreState & UserStoreActions>()(
	immer(
		devtools(
			(set, get) =>
				({
					nick: 'Guest',
					user: undefined,

					setUser: (user: UserDTO | undefined) => {
						set({
							user: user,
							nick: user?.userName ?? 'Guest'
						});
					},
					setNickname: (nick) => {
						set({
							nick: nick
						});
					}
				}) as UserStoreState & UserStoreActions,
			{ name: StoreName, store: StoreName }
		)
	)
);
export default useUserStore;
