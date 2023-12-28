import { message } from 'antd';
import { useGamesHub } from 'hubs/gamesHub';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateEffect } from 'usehooks-ts';

interface ServerConnectionNotifierProps {}

const ServerConnectionNotifier: React.FC<ServerConnectionNotifierProps> = ({}) => {
	const [messageApi, contextHolder] = message.useMessage();
	const { isConnected } = useGamesHub();
	const { t } = useTranslation();

	useUpdateEffect(() => {
		messageApi.open({
			type: isConnected ? 'success' : 'error',
			content: t(isConnected ? 'Connected to server' : 'Disconnected from sever')
		});
	}, [isConnected, messageApi]);
	return <>{contextHolder}</>;
};

export default ServerConnectionNotifier;
