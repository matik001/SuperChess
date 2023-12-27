import UserProfile from 'components/UserProfile/UserProfile';
import React from 'react';
import MainTemplatePage from './templates/MainTemplatePage';

interface UserProfilePageProps {}

const UserProfilePage: React.FC<UserProfilePageProps> = ({}) => {
	return (
		<MainTemplatePage>
			<UserProfile />
		</MainTemplatePage>
	);
};

export default UserProfilePage;
