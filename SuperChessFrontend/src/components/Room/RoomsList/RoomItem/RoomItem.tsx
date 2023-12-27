import { Button, Input } from 'antd';
import { RoomExtendedDTO } from 'api/roomApi';
import { darken } from 'polished';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { AiOutlinePlus } from 'react-icons/ai';
import { styled } from 'styled-components';
interface RoomItemProps {
	mode: 'room' | 'insert';
	room?: RoomExtendedDTO;
	onCreate?: (newName: string) => void;
	onClick?: () => void;
}
const RoomContainer = styled.div<{ transparent?: number }>`
	background-color: ${(p) => p.theme.bgColor};
	width: 200px;
	height: 200px;
	padding: 20px;
	display: flex;
	flex-direction: column;
	position: relative;
	justify-content: center;
	align-items: center;
	cursor: pointer;
	transition: all 0.5s;
	opacity: ${(p) => (p.transparent ? '0.4' : '1')};
	&:hover {
		background-color: ${(p) => darken(0.05, p.theme.bgColor)};
		opacity: 1;
	}
	font-size: 20px;
`;
const RoomItem: React.FC<RoomItemProps> = ({ mode, room, onCreate, onClick }) => {
	const { t } = useTranslation();
	const [isSettingName, setIsSettingName] = useState(false);
	const [newName, setNewName] = useState('');
	if (mode === 'insert' || room === undefined) {
		if (!isSettingName) {
			return (
				<RoomContainer transparent={1} onClick={() => setIsSettingName((prev) => !prev)}>
					<span style={{ fontSize: '50px' }}>
						<AiOutlinePlus />
					</span>
				</RoomContainer>
			);
		} else {
			const handleClickCreate = () => {
				setNewName('');
				onCreate && onCreate(newName);
			};
			return (
				<RoomContainer onClick={() => setIsSettingName((prev) => !prev)}>
					<div
						style={{
							fontSize: '20px',
							display: 'flex',
							flexDirection: 'column',
							alignItems: 'center'
						}}
					>
						<div>{t('New room')}</div>
						<Input
							autoFocus
							onClick={(e) => e.stopPropagation()}
							value={newName}
							bordered={false}
							placeholder={t('Enter a new name')}
							style={{ textAlign: 'center', marginTop: '8px', marginBottom: '20px' }}
							onChange={(e) => setNewName(e.target.value)}
							onPressEnter={handleClickCreate}
						></Input>
						<Button disabled={newName.length === 0} type="default" onClick={handleClickCreate}>
							{t('Create')}
						</Button>
					</div>
				</RoomContainer>
			);
		}
	}
	return (
		<RoomContainer onClick={onClick}>
			<span>{room.roomName}</span>
			<div
				style={{
					position: 'absolute',
					right: '16px',
					bottom: '16px',
					fontSize: '12px'
				}}
			>
				{t('X ongoing games', { count: room.amountOfUsers })}
			</div>
		</RoomContainer>
	);
};

export default RoomItem;
