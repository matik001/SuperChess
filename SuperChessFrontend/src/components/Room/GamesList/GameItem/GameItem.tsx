import { Chess } from 'chess.js';
import { GameDTO } from 'hubs/gamesHub';
import React, { useMemo, useRef } from 'react';
import { Chessboard } from 'react-chessboard';
import { useTranslation } from 'react-i18next';
import { styled } from 'styled-components';
import { useHover } from 'usehooks-ts';
interface GameItemProps {
	game: GameDTO;
	onClick: (game: GameDTO) => void;
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
	opacity: ${(p) => (p.hoveredContainer ? 0.4 : 0)};
	width: 100%;
	height: 100%;
	position: absolute;
	z-index: 10;
	cursor: pointer;
	background-color: ${(p) => p.theme.bgColor};
`;

const GameItem: React.FC<GameItemProps> = ({ game, onClick }) => {
	const fen = useMemo(() => {
		const chess = new Chess();
		chess.loadPgn(game.chessData.positionPgn);
		return chess.fen();
	}, [game.chessData.positionPgn]);
	const { t } = useTranslation();
	const white = game.userGames.find((a) => a.color === 'White');
	const black = game.userGames.find((a) => a.color === 'Black');
	const containerRef = useRef<any>();
	const hovered = useHover(containerRef);
	return (
		<Container ref={containerRef} onClick={() => onClick(game)}>
			<ChessboardBackdrop hoveredContainer={hovered ? 1 : 0}></ChessboardBackdrop>
			{hovered && (
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
			<UserStrip>{black ? black.nick : t('Waiting for a player')}</UserStrip>
			<Chessboard position={fen} arePiecesDraggable={false} boardWidth={200} />
			<UserStrip>{white ? white.nick : t('Waiting for a player')}</UserStrip>
		</Container>
	);
};

export default GameItem;
