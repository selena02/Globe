import React, { useState } from "react";
import { useMutation } from "@tanstack/react-query";

const Register = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  return (
    <form>
      <button type="submit">Register</button>
    </form>
  );
};

export default Register;
