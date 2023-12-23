// import ChatsPage from 'pages/ChatPage';
import NotFoundPage from 'pages/NotFoundPage';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

const Router = () => {
	return (
		<BrowserRouter>
			<Routes>
				{/* <Route path="/" element={<ChatsPage />} /> */}
				<Route path="*" element={<NotFoundPage />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
