import { Button, Switch } from 'antd';
import LogoImageSrc from 'assets/logo.png?url';
import { ReactNode } from 'react';
import { useTranslation } from 'react-i18next';
import { MdOutlineDarkMode, MdOutlineWbSunny } from 'react-icons/md';
import styled from 'styled-components';
import { useDarkMode } from 'usehooks-ts';
const Title = styled.h1`
	text-align: center;
	font-weight: 400;
	font-size: 32px;
	pointer-events: none;
	user-select: none;
`;
const ThemeSwitch = styled(Switch)`
	& .ant-switch-handle::before {
		background-color: black;
	}
	margin-left: 7px;
`;
const DarkModeIcon = styled(MdOutlineDarkMode)`
	font-size: 18px;
`;
const LightModeIcon = styled(MdOutlineWbSunny)`
	font-size: 18px;
`;
interface MainTemplatePageProps {
	children: ReactNode;
}
const Layout = styled.div`
	/* height: 100%; */
	min-height: 100vh;
	display: flex;
	flex-direction: column;
`;
const MenuRow = styled.div`
	min-width: 100vh;
	display: flex;
	flex-direction: row;
	align-items: center;
	padding: 16px;
	background-color: ${(props) => props.theme.secondaryColor};
	width: 100%;
`;
const MainTemplatePage = ({ children }: MainTemplatePageProps) => {
	const { isDarkMode, toggle } = useDarkMode(true);
	const { t, i18n } = useTranslation();
	return (
		<Layout>
			<MenuRow>
				<img height={40} style={{ marginRight: '5px' }} src={LogoImageSrc}></img>
				<Title>{t('AppTitle')}</Title>
				<Button
					style={{ marginLeft: 'auto' }}
					type="text"
					onClick={() => {
						i18n.changeLanguage(i18n.language === 'pl' ? 'en' : 'pl');
					}}
				>
					{i18n.language === 'pl' ? 'PL' : 'ENG'}
				</Button>
				<ThemeSwitch
					checked={isDarkMode}
					onClick={toggle}
					checkedChildren={<DarkModeIcon />}
					unCheckedChildren={<LightModeIcon />}
				/>
			</MenuRow>

			<div style={{ flex: 1, position: 'relative' }}>{children}</div>
		</Layout>
	);
};

export default MainTemplatePage;
