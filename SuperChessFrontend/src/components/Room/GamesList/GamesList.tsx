import { ScrollableMixin } from 'components/UI/Scrollable/Scrollable';
import Spinner from 'components/UI/Spinners/Spinner';
import { useGamesHub } from 'hubs/gamesHub';
import React from 'react';
import { useTranslation } from 'react-i18next';
import useUserStore from 'store/userStore';
import { styled } from 'styled-components';
import GameItem from './GameItem/GameItem';
import GameItemNew from './GameItem/GameItemNew';

interface GamesListProps {
	roomId: number;
}
const Container = styled.div`
	${ScrollableMixin}
	display: flex;
	flex-flow: row wrap;
	align-content: start;
	gap: 15px;
`;
const GamesList: React.FC<GamesListProps> = ({ roomId }) => {
	const { useGames, createGame, isConnected } = useGamesHub();
	const games = useGames();
	const nick = useUserStore((a) => a.nick);
	const { t } = useTranslation();
	return (
		<>
			{/* <h3>Room id: {roomId}</h3> */}
			{!isConnected && <Spinner tip={t('Connecting to server')} />}
			<Container>
				<GameItemNew onClick={() => createGame(roomId, 'Chess', nick)} />
				{games &&
					games.map((game) => {
						return <GameItem key={game.gameGuid} game={game} onClick={(game) => {}} />;
					})}
			</Container>
		</>
	);
};

export default GamesList;
