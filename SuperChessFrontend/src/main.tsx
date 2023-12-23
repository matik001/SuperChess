import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import React, { Suspense } from 'react';
import ReactDOM from 'react-dom/client';
import { ErrorBoundary } from 'react-error-boundary';
import { ToastContainer } from 'react-toastify';
import GlobalErrorInfo from './components/Error/GlobalErrorInfo';
import './index.css';
import LoadingPage from './pages/LoadingPage';
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
				retry: 0
			}
		}
	});

	ReactDOM.createRoot(document.getElementById('root')!).render(
		<React.StrictMode>
			<AppTranslationsProvider>
				<QueryClientProvider client={queryClient}>
					<ErrorBoundary FallbackComponent={GlobalErrorInfo}>
						<Suspense fallback={<LoadingPage />}>
							<AppThemeProvider>
								<Router />
								<ReactQueryDevtools initialIsOpen={false} />
								<ToastContainer position="bottom-right" />
							</AppThemeProvider>
						</Suspense>
					</ErrorBoundary>
				</QueryClientProvider>
			</AppTranslationsProvider>
		</React.StrictMode>
	);
};

init();
