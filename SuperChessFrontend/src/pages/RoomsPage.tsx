import React from 'react';
import useAuthStore from 'store/authStore';
import MainTemplatePage from './templates/MainTemplatePage';

interface RoomsPageProps {}

const RoomsPage: React.FC<RoomsPageProps> = ({}) => {
	const tokens = useAuthStore((a) => a.tokens);
	console.log('tokens', tokens);
	return (
		<MainTemplatePage>
			ROOMS PAGE
			<br />
			{tokens && tokens.user ? 'You are signed in as ' + tokens.user.userName : 'You are guest'}
		</MainTemplatePage>
	);
};

export default RoomsPage;
