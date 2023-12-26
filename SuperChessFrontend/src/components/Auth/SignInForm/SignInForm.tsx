import { Button, Spin } from 'antd';
import ValidatedInput from 'components/UI/ValidatedInput';
import React from 'react';
import { useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import useAuthStore from 'store/authStore';
import { styled } from 'styled-components';

interface SignInFormProps {}

const LoginPanel = styled.div`
	padding: 10px 60px 30px 60px;
	background-color: ${(p) => p.theme.secondaryColor};
	width: 600px;
	margin: 100px auto;
	max-width: 90%;
`;
const Title = styled.div`
	font-size: 36px;
	padding: 20px 0 40px 0;
	font-weight: 300;
`;
interface FormData {
	Username: string;
	Password: string;
}
const SignInForm: React.FC<SignInFormProps> = ({}) => {
	const signin = useAuthStore((a) => a.signin);
	const isLoading = useAuthStore((a) => a.isLoading);

	const {
		register,
		handleSubmit,
		formState: { errors },
		control
	} = useForm<FormData>();
	const onSubmit = (data: FormData) => {
		signin({
			password: data.Password,
			userName: data.Username
		});
	};
	const { t } = useTranslation();
	const navigate = useNavigate();
	return (
		<LoginPanel>
			<Spin spinning={isLoading}>
				<Title>{t('Sign in')}</Title>
				<form onSubmit={handleSubmit(onSubmit)}>
					<div style={{ marginBottom: '9px' }}>{t('Username')}: </div>
					<ValidatedInput
						control={control}
						name="Username"
						style={{ marginBottom: '24px' }}
						rules={{}}
					/>

					<div style={{ marginBottom: '9px' }}>{t('Password')}: </div>
					<ValidatedInput
						control={control}
						name="Password"
						style={{ marginBottom: '24px' }}
						rules={{}}
						isPassword
					/>

					<Button type="primary" htmlType="submit">
						{t('Sign in')}
					</Button>
					<Button type="link" onClick={() => navigate('/signup')}>
						{t('Dont have account? Sign up.')}
					</Button>
				</form>
			</Spin>
		</LoginPanel>
	);
};

export default SignInForm;
