import axios from 'axios';
import { toast } from 'react-toastify';

export const appAxios = axios.create({
	baseURL: '/api'
});

appAxios.interceptors.request.use(
	function (config) {
		return config;
	},
	function (error) {
		if (!axios.isCancel(error)) {
			toast.error(`Something went wrong: ${(error as Error).message}`); /// TODO dodac tlumaczenie
			console.error(error);
		}
		return Promise.reject(error);
	}
);

appAxios.interceptors.response.use(
	function (response) {
		return response;
	},
	function (error) {
		if (!axios.isCancel(error)) {
			toast.error(`Something went wrong: ${(error as Error).message}`); /// TODO dodac tlumaczenie
			console.error(error);
		}

		return Promise.reject(error);
	}
);
