import { Spin } from 'antd';
import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import React, { ReactNode, useEffect, useState } from 'react';
import { initReactI18next } from 'react-i18next';

interface AppTranslationsProviderProps {
	children: ReactNode;
}

const resources = {
	en: {
		translation: {
			AppTitle: 'SuperAGI',
			Value: 'Value',
			Refresh: 'Refresh',
			Logs: 'Logs',
			NotFound: 'Not found',
			BackHome: 'Back home',
			SendMessage: 'Send a message.',
			assistant: 'Assistant',
			user: 'User',
			system: 'System',
			Add: 'Add'
		}
	},
	pl: {
		translation: {
			AppTitle: 'SuperAGI',
			Value: 'Wartość',
			Refresh: 'Odśwież',
			Logs: 'Logi',
			NotFound: 'Nie znaleziono',
			BackHome: 'Powrót na stronę główną',
			SendMessage: 'Wyślij wiadomość.',
			assistant: 'Asystent',
			user: 'Użytkownik',
			system: 'System',
			Add: 'Dodaj'
		}
	}
};
declare module 'i18next' {
	interface CustomTypeOptions {
		defaultNS: 'en';
		resources: (typeof resources)['en'];
	}
}
const AppTranslationsProvider: React.FC<AppTranslationsProviderProps> = ({ children }) => {
	const [isLoading, setIsLoading] = useState(true);
	useEffect(() => {
		i18next.use(initReactI18next).use(LanguageDetector).init({
			fallbackLng: 'en',
			resources: resources
		});
		setIsLoading(false);
	}, []);

	return <>{isLoading ? <Spin /> : children}</>;
};

export default AppTranslationsProvider;
