import { Outlet } from "react-router-dom";
import "./Account.scss";

const Account = () => {
  return (
    <div id="account-container">
      <Outlet />
    </div>
  );
};

export default Account;
