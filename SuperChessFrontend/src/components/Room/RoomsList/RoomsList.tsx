import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { QUERYKEY_GETROOMS, createRoom, getRooms } from 'api/roomApi';
import { ScrollableMixin } from 'components/UI/Scrollable/Scrollable';
import Spinner from 'components/UI/Spinners/Spinner';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import useAuthStore from 'store/authStore';
import { styled } from 'styled-components';
import RoomItem from './RoomItem/RoomItem';
interface RoomsListProps {}

const Container = styled.div`
	${ScrollableMixin}
	display: flex;
	background-color: ${(p) => p.theme.secondaryColor};
	flex-wrap: wrap;
	padding: 30px;
	height: 90%;
	margin: 30px 30px 0px 30px;
	gap: 30px;
	height: calc(100% - 30px);
	align-content: start;
`;

const RoomsList: React.FC<RoomsListProps> = ({}) => {
	const user = useAuthStore((a) => a.tokens?.user);
	const { data: rooms, isFetching } = useQuery({
		queryKey: [QUERYKEY_GETROOMS],
		queryFn: ({ signal }) => {
			return getRooms(signal);
		}
	});
	const navigate = useNavigate();
	const { t } = useTranslation();
	const queryClient = useQueryClient();
	const addRoomMutation = useMutation({
		mutationFn: async (newName: string) => {
			const newRoom = await createRoom({
				roomName: newName
			});
			return newRoom;
		},
		onSuccess: (newRoom) => {
			queryClient.invalidateQueries({
				queryKey: [QUERYKEY_GETROOMS]
			});
		}
	});
	return (
		<Container>
			{rooms &&
				rooms.map((room) => (
					<RoomItem
						mode="room"
						room={room}
						key={room.id}
						onClick={() => {
							navigate(`/rooms/${room.id}`);
						}}
					/>
				))}

			<RoomItem mode="insert" onCreate={addRoomMutation.mutate} />
			{isFetching || addRoomMutation.isPending ? <Spinner /> : null}
		</Container>
	);
};

export default RoomsList;
