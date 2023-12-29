import { ScrollableMixin } from 'components/UI/Scrollable/Scrollable';
import Spinner from 'components/UI/Spinners/Spinner';
import { useGamesHub } from 'hubs/gamesHub';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
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
	const [games, setGames] = useGames();
	const nick = useUserStore((a) => a.nick);
	const user = useUserStore((a) => a.user);
	const guestGuid = useUserStore((a) => a.guestGuid);
	const { t } = useTranslation();
	const navigate = useNavigate();
	const onCreateGame = async () => {
		const guid = await createGame(guestGuid, nick, roomId, 'Chess');
		navigate(`/game/${guid}`);
	};
	return (
		<>
			{!isConnected && <Spinner tip={t('Connecting to server')} />}
			<Container>
				<GameItemNew onClick={onCreateGame} />
				{games &&
					games.map((game) => {
						const areYouInGame = game.userGames.some(
							(a) =>
								(guestGuid && a.guestGuid === guestGuid) || (a.user?.id && a.user?.id === user?.id)
						);
						return (
							<GameItem
								key={game.gameGuid}
								game={game}
								// canJoin={!areYouInGame}
								canJoin={true}
								onClick={(game) => {
									navigate(`/game/${game.gameGuid}`);
								}}
							/>
						);
					})}
			</Container>
		</>
	);
};

export default GamesList;
