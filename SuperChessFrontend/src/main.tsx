import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import React from 'react';
import ReactDOM from 'react-dom/client';
import { ToastContainer } from 'react-toastify';
import './index.css';
import AppThemeProvider from './setup/AppThemeProvider';
import AppTranslationsProvider from './setup/AppTranslationsProvider';
import Router from './setup/router/router';

const init = async () => {
	const queryClient = new QueryClient({
		defaultOptions: {
			queries: {
				refetchOnReconnect: false,
				refetchOnWindowFocus: false,
				refetchInterval: false,
				refetchOnMount: false,
				retry: 3
			}
		}
	});

	ReactDOM.createRoot(document.getElementById('root')!).render(
		<React.StrictMode>
			<AppTranslationsProvider>
				<QueryClientProvider client={queryClient}>
					{/* <ErrorBoundary FallbackComponent={}> */}
					<AppThemeProvider>
						<Router />
						<ReactQueryDevtools initialIsOpen={false} />
						<ToastContainer position="bottom-right" />
					</AppThemeProvider>
					{/* </ErrorBoundary> */}
				</QueryClientProvider>
			</AppTranslationsProvider>
		</React.StrictMode>
	);
};

init();
