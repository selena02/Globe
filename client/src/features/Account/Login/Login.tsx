import { useState } from "react";
import { useForm } from "react-hook-form";
import { LoginDto } from "../models/LoginDto";
import { AuthResponse } from "../models/AuthResponse";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import "./Login.scss";
import { Link, useNavigate } from "react-router-dom";
import {
  setLogin,
  setLoading,
  setError,
} from "../../../state/features/authSlice";
import { useDispatch } from "react-redux";

const Login = () => {
  const {
    register,
    handleSubmit,
    formState: { errors, isValid, isSubmitting },
  } = useForm<LoginDto>({
    mode: "onChange",
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const onSubmit = async (data: LoginDto) => {
    setIsLoading(true);
    dispatch(setLoading(true));

    try {
      const response = await fetchAPI<AuthResponse>("Authentication/login", {
        method: "POST",
        body: data,
      });
      localStorage.setItem("token", response.token);
      localStorage.setItem("user", JSON.stringify(response));
      dispatch(setLogin({ user: response, token: response.token }));
      navigate("/");
    } catch (error: any) {
      handleApiErrors(error);
      dispatch(setError(error.message));
    } finally {
      setIsLoading(false);
      dispatch(setLoading(false));
    }
  };

  return (
    <form id="login-form" onSubmit={handleSubmit(onSubmit)}>
      <div id="sign-in-container">
        <img src="/images/general/logo.png" alt="Logo" />
        <h1>Sign in</h1>
      </div>
      <label htmlFor="email">Email*</label>
      <input
        id="email"
        type="email"
        {...register("email", {
          required: "Email is required",
          pattern: {
            value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
            message: "Invalid email address",
          },
        })}
      />
      <p className="error">{errors.email ? errors.email.message : ""}</p>

      <label htmlFor="password">Password*</label>
      <input
        id="password"
        type="password"
        {...register("password", {
          required: "Password is required",
        })}
      />
      <p className="error">{errors.password ? errors.password.message : ""}</p>

      <button type="submit" disabled={isLoading || isSubmitting || !isValid}>
        {!isLoading ? "Login" : "Loading..."}
      </button>

      <Link to="/account/register">Create an account</Link>
    </form>
  );
};

export default Login;
