import LogoImageSrc from 'assets/logo.png?url';
import TopMenu from 'components/TopMenu/TopMenu';
import { ReactNode } from 'react';
import { useTranslation } from 'react-i18next';
import styled from 'styled-components';
const Title = styled.h1`
	text-align: center;
	font-weight: 400;
	font-size: 32px;
	pointer-events: none;
	user-select: none;
`;

interface MainTemplatePageProps {
	children: ReactNode;
}
const Layout = styled.div`
	height: 100vh;
	width: 100%;
	display: flex;
	flex-direction: column;
	position: relative;
	overflow: hidden;
`;
const MenuRow = styled.div`
	display: flex;
	flex-direction: row;
	align-items: center;
	padding: 16px;
	background-color: ${(props) => props.theme.secondaryColor};
	width: 100%;
`;
const MainTemplatePage = ({ children }: MainTemplatePageProps) => {
	const { t } = useTranslation();
	return (
		<Layout>
			<MenuRow>
				<img height={40} style={{ marginRight: '5px' }} src={LogoImageSrc}></img>
				<Title>{t('App title')}</Title>
				<TopMenu />
			</MenuRow>

			<div
				style={{
					flex: 1,
					position: 'relative',
					width: '100%',
					flexGrow: '1',
					minHeight: 0
				}}
			>
				{children}
			</div>
		</Layout>
	);
};

export default MainTemplatePage;
