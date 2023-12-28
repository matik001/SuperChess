import GamesList from 'components/Room/GamesList/GamesList';
import { useGamesHub } from 'hubs/gamesHub';
import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import MainTemplatePage from './templates/MainTemplatePage';

interface GamesPageProps {}

const GamesPage: React.FC<GamesPageProps> = ({}) => {
	const { roomId: roomIdStr } = useParams();
	const roomId = parseInt(roomIdStr!);

	const { joinRoom, leaveRoom, isConnected } = useGamesHub();
	useEffect(() => {
		isConnected && joinRoom(roomId);
		return () => {
			isConnected && leaveRoom(roomId);
		};
	}, [isConnected, joinRoom, leaveRoom, roomId]);

	return (
		<MainTemplatePage
			style={{
				margin: '15px',
				width: 'calc(100% - 30px)'
			}}
		>
			<GamesList roomId={roomId} />
		</MainTemplatePage>
	);
};

export default GamesPage;
