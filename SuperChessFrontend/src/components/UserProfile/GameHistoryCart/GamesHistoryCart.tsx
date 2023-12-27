import { Card } from 'antd';
import { t } from 'i18next';
import React from 'react';
import { Trans } from 'react-i18next';
import { Link } from 'react-router-dom';
import useUserStore from 'store/userStore';

interface GamesHistoryCartProps {}

const GamesHistoryCart: React.FC<GamesHistoryCartProps> = ({}) => {
	const user = useUserStore((a) => a.user);

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
				{user ? null : (
					<div style={{ marginBottom: '50px' }}>
						<Trans i18nKey="You must <0>sign in</0> to keep your games history.">
							<Link to="/signin">sign in</Link>
						</Trans>
					</div>
				)}
			</Card>
		</>
	);
};

export default GamesHistoryCart;
