import { GameDTO } from 'api/gameApi';
import React, { useRef } from 'react';
import { Chessboard } from 'react-chessboard';
import { useTranslation } from 'react-i18next';
import { styled } from 'styled-components';
import { useHover } from 'usehooks-ts';
interface GameItemProps {
	game: GameDTO;
	onClick: (game: GameDTO) => void;
	canJoin: boolean;
}

const Container = styled.div`
	position: relative;
	user-select: none;
	cursor: pointer;
`;
const UserStrip = styled.div`
	background-color: ${(a) => a.theme.secondaryColor};
	height: 40px;
	display: flex;
	align-items: center;
	justify-content: center;
`;
const ChessboardBackdrop = styled.div<{ hoveredContainer: 1 | 0 }>`
	transition: all 0.3s;
	opacity: ${(p) => (p.hoveredContainer ? 0.4 : 0)};
	width: 100%;
	height: 100%;
	position: absolute;
	z-index: 10;
	background-color: ${(p) => p.theme.bgColor};
`;

const GameItem: React.FC<GameItemProps> = ({ game, onClick, canJoin }) => {
	const { t } = useTranslation();
	const white = game.userGames.find((a) => a.color === 'White');
	const black = game.userGames.find((a) => a.color === 'Black');
	const containerRef = useRef<any>();
	const hovered = useHover(containerRef);
	return (
		<Container ref={containerRef} onClick={() => onClick(game)}>
			<ChessboardBackdrop hoveredContainer={hovered && canJoin ? 1 : 0}></ChessboardBackdrop>
			{hovered && canJoin && (
				<div
					style={{
						zIndex: 14,
						fontSize: 20,
						fontWeight: 'bold',
						position: 'absolute',
						display: 'flex',
						alignItems: 'center',
						justifyContent: 'center',
						width: '100%',
						height: '100%'
					}}
				>
					{t('Click to join')}
				</div>
			)}
			<UserStrip>{black ? black.nick : t('Waiting for an opponent')}</UserStrip>
			<Chessboard
				position={game.chessData.positionFEN}
				arePiecesDraggable={false}
				boardWidth={200}
			/>
			<UserStrip>{white ? white.nick : t('Waiting for an opponent')}</UserStrip>
		</Container>
	);
};

export default GameItem;
