import { useQuery } from '@tanstack/react-query';
import { Card } from 'antd';
import { QUERYKEY_GETGAMES, getGames } from 'api/gameApi';
import { ScrollableMixin } from 'components/UI/Scrollable/Scrollable';
import Spinner from 'components/UI/Spinners/Spinner';
import { t } from 'i18next';
import React from 'react';
import { Trans } from 'react-i18next';
import { Link, useNavigate } from 'react-router-dom';
import useUserStore from 'store/userStore';
import { styled } from 'styled-components';
import GamesHistoryItem from './GamesHistoryItem/GamesHistoryItem';

interface GamesHistoryCartProps {}

const Container = styled.div`
	${ScrollableMixin}
	display: flex;
	flex-flow: row wrap;
	align-content: start;
	gap: 15px;
`;
const GamesHistoryCart: React.FC<GamesHistoryCartProps> = ({}) => {
	const user = useUserStore((a) => a.user);
	const gamesQuery = useQuery({
		queryKey: [QUERYKEY_GETGAMES],
		queryFn: async ({ signal }) => {
			return await getGames(signal);
		},
		enabled: !!user
	});
	const navigate = useNavigate();
	return (
		<>
			<Card
				title={t('Games history')}
				bodyStyle={{
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
					height: '100%'
				}}
				style={{ flexGrow: '1' }}
			>
				{gamesQuery.isFetching && <Spinner />}
				<div style={{ marginBottom: '50px' }}>
					{user ? null : (
						<Trans i18nKey="You must <0>sign in</0> to keep your games history.">
							<Link to="/signin">sign in</Link>
						</Trans>
					)}
					{gamesQuery.data && gamesQuery.data.length === 0 && t('Yours games history is empty')}
				</div>
				<Container
					style={{
						height: 'calc(100% - 60px)',
						marginTop: '-70px'
					}}
				>
					{gamesQuery.data?.map((game) => (
						<GamesHistoryItem
							key={game.gameGuid}
							canJoin={true}
							game={game}
							onClick={(game) => {
								navigate(`/game/${game.gameGuid}`);
							}}
						/>
					))}
				</Container>
			</Card>
		</>
	);
};

export default GamesHistoryCart;
