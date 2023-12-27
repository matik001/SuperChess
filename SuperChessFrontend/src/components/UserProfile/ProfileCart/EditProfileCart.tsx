import { Button, Card, Input, Space } from 'antd';
import { t } from 'i18next';
import React, { useCallback, useMemo, useState } from 'react';
import useUserStore from 'store/userStore';
import { useBoolean } from 'usehooks-ts';

interface EditProfileCartProps {}

const EditProfileCart: React.FC<EditProfileCartProps> = ({}) => {
	const nickname = useUserStore((a) => a.nick);
	const setNickname = useUserStore((a) => a.setNickname);
	const { value: isEditingName, toggle: toggleIsEditingName } = useBoolean();
	const [newNickname, setNewNickname] = useState(nickname);

	const canSave = useMemo(() => newNickname !== nickname, [newNickname, nickname]);
	const onSave = useCallback(() => {
		setNickname(newNickname);
	}, [newNickname, setNickname]);

	const isSignedIn = !!useUserStore((a) => a.user);

	return (
		<>
			<Card
				title={t('Your profile')}
				extra={
					<>
						<Button type="primary" disabled={!canSave} onClick={onSave}>
							{t('Save')}
						</Button>
					</>
				}
			>
				<Space direction="horizontal">
					<span>{t('Your nickname')}:</span>
					{isEditingName ? (
						<Input
							autoFocus
							onChange={(e) => setNewNickname(e.target.value)}
							value={newNickname}
							onBlur={toggleIsEditingName}
							onPressEnter={toggleIsEditingName}
						/>
					) : (
						<>
							<span style={{ fontWeight: 'bold' }}>{newNickname}</span>
							{!isSignedIn && (
								<Button type="default" onClick={toggleIsEditingName}>
									{t('Change')}
								</Button>
							)}
						</>
					)}
				</Space>
			</Card>
		</>
	);
};

export default EditProfileCart;
