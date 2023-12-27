import { useTranslation } from 'react-i18next';
import { styled } from 'styled-components';
import GamesHistoryCart from './GameHistoryCart/GamesHistoryCart';
import EditProfileCart from './ProfileCart/EditProfileCart';

export const Container = styled.div`
	display: flex;
	flex-direction: row;
	height: calc(100% - 100px);
	margin: 50px;
	gap: 30px;
`;
export interface UserProfileProps {}

export const UserProfile: React.FC<UserProfileProps> = ({}) => {
	const { t } = useTranslation();

	return (
		<Container>
			<EditProfileCart />
			<GamesHistoryCart />
		</Container>
	);
};

export default UserProfile;
