import { useState } from "react";
import { useForm } from "react-hook-form";
import { RegisterDtoForm } from "../models/RegisterDto"; // Assuming you have a RegisterDto model
import { AuthResponse } from "../models/AuthResponse";
import fetchAPI from "../../../shared/utils/fetchAPI";
import { handleApiErrors } from "../../../shared/utils/displayApiErrors";
import "./Register.scss";
import { Link } from "react-router-dom";

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

  const onSubmit = async (data: RegisterDtoForm) => {
    setIsLoading(true);
    setPasswordError("");

    try {
      const { confirmPassword, ...requestData } = data;
      const response = await fetchAPI<AuthResponse>("register", {
        method: "POST",
        body: data,
      });
    } catch (error: any) {
      handleApiErrors(error);
    } finally {
      setIsLoading(false);
    }
  };

  const password = watch("password", "");

  return (
    <form id="register-form" onSubmit={handleSubmit(onSubmit)}>
      <div id="sign-up-container">
        <img src="/images/general/logo.png" alt="Logo" />
        <h1>Sign up</h1>
      </div>
      <label htmlFor="fullName">Full Name*</label>
      <input
        id="fullName"
        type="text"
        {...register("fullName", {
          required: "Full Name is required",
        })}
      />
      <p className="error">{errors.fullName ? errors.fullName.message : ""}</p>

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
