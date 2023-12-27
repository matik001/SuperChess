import GamesList from 'components/Room/GamesList/GamesList';
import React from 'react';
import { useParams } from 'react-router-dom';
import MainTemplatePage from './templates/MainTemplatePage';

interface GamesPageProps {}

const GamesPage: React.FC<GamesPageProps> = ({}) => {
	const { roomId: roomIdStr } = useParams();
	const roomId = parseInt(roomIdStr!);

	return (
		<MainTemplatePage>
			<GamesList roomId={roomId} />
		</MainTemplatePage>
	);
};

export default GamesPage;
