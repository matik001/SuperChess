import { Spin } from 'antd';
import { GameDTO, PlayerColors } from 'api/gameApi';
import blackPawnSvg from 'assets/black_pawn.svg?url';
import whitePawnSvg from 'assets/white_pawn.svg?url';
import { Chess } from 'chess.js';

import { Move, pieceSymbolToType, positionFromString } from 'hubs/gamesHub';
import React, { useMemo } from 'react';
import { Chessboard } from 'react-chessboard';
import { Piece, Square } from 'react-chessboard/dist/chessboard/types';
import { useTranslation } from 'react-i18next';
import { styled } from 'styled-components';
import { useElementSize } from 'usehooks-ts';
interface ChessGameProps {
	isObservator: boolean;
	game: GameDTO;
	perspective: PlayerColors;
	onMove: (move: Move) => void;
}

const Container = styled.div`
	display: flex;
	background-color: ${(p) => p.theme.secondaryColor};
	padding: 30px;
	height: 100%;
	margin: auto;
	width: 100%;
	max-width: 1200px;
	flex-direction: row;
	justify-content: center;
	align-items: center;
	position: relative;
	gap: 20px;
`;
const RightContainer = styled.div`
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: space-between;
	height: 100%;
	user-select: none;
	pointer-events: none;
	flex-grow: 1;
	min-width: 0;
`;
const CurrentPlayerMoveContainer = styled.div`
	display: flex;
	flex-direction: row;
	align-items: center;
	align-self: start;
	margin-bottom: 50px;
	margin-top: 50px;
	margin-left: -15px;
`;
const ChessGame: React.FC<ChessGameProps> = ({ isObservator, game, perspective, onMove }) => {
	const [containerRef, { width, height }] = useElementSize();
	const { t } = useTranslation();
	const gameEngine = useMemo(
		() => new Chess(game.chessData.positionFEN),
		[game.chessData.positionFEN]
	);
	function onDrop(sourceSquare: Square, targetSquare: Square, piece: Piece) {
		const move = gameEngine.move({
			from: sourceSquare,
			to: targetSquare,
			promotion: 'q'
		});

		if (move === null) return false;
		onMove({
			from: positionFromString(move.from),
			to: positionFromString(move.to),
			promotion: move.promotion ? pieceSymbolToType(move.promotion) : undefined
		});

		return true;
	}
	const isOurMove =
		(gameEngine.turn() === 'w' && perspective === 'White') ||
		(gameEngine.turn() === 'b' && perspective === 'Black');

	const isWaitingForOpponent = game.userGames.length < 2;
	const showWhoseMoveIs = !isWaitingForOpponent && game.gameStatus === 'InProgress';
	const playerBottom = game.userGames.find((a) => a.color === perspective);
	const playerTop = game.userGames.find((a) => a.color !== perspective);
	return (
		<Container ref={containerRef}>
			<div
				style={{
					position: 'relative',
					display: 'flex',
					flexDirection: 'column',
					gap: '20px',
					justifyContent: 'space-between'
				}}
			>
				<div style={{ position: 'absolute', left: height - 60 + 10, whiteSpace: 'nowrap' }}>
					{playerTop ? playerTop.nick + ' (1200)' : t('Waiting for an opponent')}
				</div>
				<Chessboard
					position={game.chessData.positionFEN}
					arePiecesDraggable={!isObservator && isOurMove}
					boardWidth={height - 60}
					boardOrientation={perspective === 'White' ? 'white' : 'black'}
					onPieceDrop={onDrop}
					autoPromoteToQueen={true}
				/>
				<div
					style={{ position: 'absolute', left: height - 60 + 10, bottom: 0, whiteSpace: 'nowrap' }}
				>
					{playerBottom ? playerBottom.nick + ' (1200)' : t('Waiting for an opponent')}
				</div>
			</div>
			<RightContainer>
				<CurrentPlayerMoveContainer
					style={{ visibility: showWhoseMoveIs && !isOurMove ? 'visible' : 'hidden' }}
				>
					<img
						style={{ width: '100px' }}
						src={perspective === 'Black' ? whitePawnSvg : blackPawnSvg}
					/>
					<div style={{ fontSize: '30px' }}>
						{perspective === 'Black' ? t('White on move') : t('Black on move')}
					</div>
				</CurrentPlayerMoveContainer>
				<div
					style={{
						fontSize: '26px',
						display: 'flex',
						justifyContent: 'center',
						alignItems: 'center',
						gap: '20px'
					}}
				>
					{isWaitingForOpponent && (
						<>
							<Spin size="large" />
							{t('Waiting for an opponent')}
						</>
					)}
					{game.chessData.result == 'WinWhite' && t('White won') + '!'}
					{game.chessData.result == 'WinBlack' && t('Black won') + '!'}
					{game.chessData.result == 'Draw' && t('Draw')}
				</div>
				<CurrentPlayerMoveContainer
					style={{
						visibility: showWhoseMoveIs && isOurMove ? 'visible' : 'hidden'
					}}
				>
					<img
						style={{ width: '100px' }}
						src={perspective === 'White' ? whitePawnSvg : blackPawnSvg}
					/>
					<div style={{ fontSize: '30px', marginLeft: '-10px' }}>
						{perspective === 'White' ? t('White on move') : t('Black on move')}
					</div>
				</CurrentPlayerMoveContainer>
			</RightContainer>
		</Container>
	);
};

export default ChessGame;
