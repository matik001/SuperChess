import RoomsList from 'components/Room/RoomsList/RoomsList';
import React from 'react';
import MainTemplatePage from './templates/MainTemplatePage';

interface RoomsPageProps {}

const RoomsPage: React.FC<RoomsPageProps> = ({}) => {
	return (
		<MainTemplatePage>
			<RoomsList />
		</MainTemplatePage>
	);
};

export default RoomsPage;
