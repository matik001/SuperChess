// import ChatsPage from 'pages/ChatPage';
import GamesPage from 'pages/GamesPage';
import NotFoundPage from 'pages/NotFoundPage';
import RoomsPage from 'pages/RoomsPage';
import SignInPage from 'pages/SignInPage';
import SignUpPage from 'pages/SignUpPage';
import UserProfilePage from 'pages/UserProfilePage';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import useAuthStore from 'store/authStore';

const Router = () => {
	const tokens = useAuthStore((a) => a.tokens);
	const isLoggedIn = !!tokens;
	console.log(tokens);
	return (
		<BrowserRouter>
			<Routes>
				{isLoggedIn ? (
					<>
						<Route path="/signin" element={<Navigate to="/" />} />
						<Route path="/signup" element={<Navigate to="/" />} />
					</>
				) : (
					<>
						<Route path="/signin" element={<SignInPage />} />
						<Route path="/signup" element={<SignUpPage />} />
					</>
				)}
				<Route path="/rooms" element={<RoomsPage />} />
				<Route path="/rooms/:roomId" element={<GamesPage />} />
				<Route path="/profile" element={<UserProfilePage />} />
				<Route path="/" element={<Navigate to="/rooms" />} />

				<Route path="*" element={<NotFoundPage />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
