import {
	UserSignInRequestDTO,
	UserSignUpRequestDTO,
	UserTokensDTO,
	refreshToken,
	signin,
	signup
} from 'api/authApi';
import { useInterval } from 'usehooks-ts';
import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { immer } from 'zustand/middleware/immer';
export interface AuthState {
	tokens?: UserTokensDTO;
	isLoading: boolean;
}
export interface AuthActions {
	updateTokens: (tokens?: UserTokensDTO) => void;
	setLoading: (isLoading: boolean) => void;
	refreshToken: () => Promise<boolean>;
	signup: (signupData: UserSignUpRequestDTO) => Promise<UserTokensDTO>;
	signin: (signinData: UserSignInRequestDTO) => Promise<UserTokensDTO>;
	logout: () => Promise<void>;
}
const StoreName = 'AuthStore';
const useAuthStore = create<AuthState & AuthActions>()(
	persist(
		immer(
			devtools(
				(set, get) =>
					({
						tokens: undefined,
						isLoading: false,

						updateTokens: (tokens) => {
							set({
								tokens: tokens,
								isLoading: false
							});
						},
						setLoading: (isLoading) => {
							set({
								isLoading: isLoading
							});
						},
						refreshToken: async () => {
							console.log('refreshing token');

							const tokens = get().tokens;
							if (!tokens) return;
							if (tokens.refreshTokenExpiration < new Date()) {
								set({
									isLoading: false,
									tokens: undefined
								});
								return;
							}
							// set({
							// 	isLoading: true
							// });
							const newTokens = await refreshToken({
								refreshToken: tokens.refreshToken,
								token: tokens.token
							});
							set({
								isLoading: false,
								tokens: newTokens
							});
							return true;
						},
						logout: async () => {
							set({
								isLoading: false,
								tokens: undefined
							});
						},
						signin: async (signinData) => {
							set({ isLoading: true });
							const tokens = await signin(signinData);
							set({
								isLoading: false,
								tokens: tokens
							});
							return tokens;
						},
						signup: async (signupData) => {
							set({ isLoading: true });
							const tokens = await signup(signupData);
							set({
								isLoading: false,
								tokens: tokens
							});
							return tokens;
						}
					}) as AuthState & AuthActions,
				{ name: StoreName, store: StoreName }
			)
		),
		{
			name: StoreName,
			onRehydrateStorage: (state) => (state) => {
				useAuthStore.getState().refreshToken();
				useInterval(() => useAuthStore.getState().refreshToken(), 300); /// refresh token every 5 min
			},
			partialize: (state) => ({
				tokens: state.tokens
			})
		}
	)
);
export default useAuthStore;
