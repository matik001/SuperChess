import ChessGame from 'components/GameTypes/ChessGame/ChessGame';
import Spinner from 'components/UI/Spinners/Spinner';
import { useGamesHub } from 'hubs/gamesHub';
import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import useUserStore from 'store/userStore';
import MainTemplatePage from './templates/MainTemplatePage';

interface ChessGamePageProps {}

const ChessGamePage: React.FC<ChessGamePageProps> = ({}) => {
	const { gameGuid } = useParams();
	const guestGuid = useUserStore((a) => a.guestGuid);
	const nick = useUserStore((a) => a.nick);
	const { joinGame, leaveRoom, useGame, makeMove, isConnected } = useGamesHub();
	const userId = useUserStore((a) => a.user?.id);
	const [game, setGame] = useGame(gameGuid!);

	useEffect(() => {
		isConnected && joinGame(guestGuid, nick, gameGuid!);
		return () => {
			// isConnected && leaveGame(roomId);
		};
	}, [gameGuid, guestGuid, isConnected, joinGame, leaveRoom, nick]);

	const playerPerspective = game?.userGames?.find(
		(a) => (a.guestGuid && a.guestGuid === guestGuid) || (a.user?.id && a.user.id === userId)
	);
	return (
		<MainTemplatePage
			style={{
				margin: '15px',
				width: 'calc(100% - 30px)',
				height: 'calc(100% - 30px)'
			}}
		>
			{!game ? (
				<Spinner />
			) : (
				<ChessGame
					game={game}
					isObservator={playerPerspective === undefined || game.gameStatus !== 'InProgress'}
					perspective={playerPerspective?.color ?? 'White'}
					onMove={(move) => {
						makeMove(guestGuid, gameGuid!, move);
					}}
				/>
			)}
		</MainTemplatePage>
	);
};

export default ChessGamePage;
