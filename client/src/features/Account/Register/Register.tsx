import { useState } from "react";
import { useForm } from "react-hook-form";
import { RegisterDtoForm } from "../models/RegisterDto"; // Assuming you have a RegisterDto model
import { AuthResponse } from "../models/AuthResponse";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import "./Register.scss";
import { Link, useNavigate } from "react-router-dom";
import {
  setLogin,
  setLoading,
  setError,
} from "../../../state/features/authSlice";
import { useDispatch } from "react-redux";

const Register = () => {
  const {
    register,
    handleSubmit,
    formState: { errors, isValid, isSubmitting },
    watch,
  } = useForm<RegisterDtoForm>({
    mode: "onChange",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [passwordError, setPasswordError] = useState("");
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const onSubmit = async (data: RegisterDtoForm) => {
    setIsLoading(true);
    setPasswordError("");
    dispatch(setLoading(true));

    try {
      const { confirmPassword, ...requestData } = data;
      const response = await fetchAPI<AuthResponse>("Authentication/register", {
        method: "POST",
        body: requestData,
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

  const password = watch("password", "");

  const validateFullName = (fullName: string) => {
    const parts = fullName.trim().split(/\s+/);
    if (parts.length < 2 || parts.length > 4) {
      return "Full name must be between 2 to 4 words";
    }
    for (let part of parts) {
      if (part.length < 2 || part.length > 15) {
        return "Must be valid name";
      }
    }
    return true;
  };

  return (
    <form id="register-form" onSubmit={handleSubmit(onSubmit)}>
      <div id="sign-up-container">
        <img src="/images/general/logo.png" alt="Logo" />
        <h1>Sign up</h1>
      </div>
      <label htmlFor="userName">Username*</label>
      <input
        id="userName"
        type="text"
        {...register("userName", {
          required: "Username is required",
          minLength: {
            value: 5,
            message: "Username too short",
          },
          maxLength: {
            value: 15,
            message: "Username too long",
          },
          pattern: {
            value: /^[a-zA-Z0-9_]*$/,
            message: "No special characters",
          },
        })}
      />
      <p className="error">{errors.userName ? errors.userName.message : ""}</p>

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
      <label htmlFor="fullName">Full Name*</label>
      <input
        id="fullName"
        type="text"
        {...register("fullName", {
          required: "Full Name is required",
          validate: validateFullName,
        })}
      />
      <p className="error">{errors.fullName ? errors.fullName.message : ""}</p>

      <label htmlFor="password">Password*</label>
      <input
        id="password"
        type="password"
        {...register("password", {
          required: "Password is required",
          minLength: {
            value: 6,
            message: "Password Too Short",
          },
        })}
      />
      <p className="error">{errors.password ? errors.password.message : ""}</p>

      <label htmlFor="confirmPassword">Confirm Password*</label>
      <input
        id="confirmPassword"
        type="password"
        {...register("confirmPassword", {
          required: "Please confirm your password",
          validate: (value) => value === password || "Passwords do not match",
        })}
      />
      <p className="error">
        {errors.confirmPassword ? errors.confirmPassword.message : ""}
      </p>
      {passwordError && <p className="error">{passwordError}</p>}

      <button type="submit" disabled={isLoading || isSubmitting || !isValid}>
        {!isLoading ? "Register" : "Loading..."}
      </button>

      <Link to="/account/login">Back To Sign In</Link>
    </form>
  );
};

export default Register;
