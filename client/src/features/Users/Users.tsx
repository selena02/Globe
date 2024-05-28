import "./Users.scss";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import fetchAPI from "../../shared/utils/fetchAPI";
import { UserDto } from "./models/user";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import Spinner from "../../shared/components/Spinner/Spinner";
import User from "./User/User"; // Import the new User component

const Users = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [users, setUsers] = useState<UserDto[]>([]);
  const [isPilot, setIsPilot] = useState(false);
  const navigate = useNavigate();
  const user = localStorage.getItem("user");

  if (!user) {
    navigate("/account/login");
  }

  const userParsed = JSON.parse(user!);
  const isGuide = userParsed.roles.includes("Guide");

  if (!isGuide) {
    navigate("/account/login");
  }

  useEffect(() => {
    const fetchUsers = async () => {
      setIsLoading(true);
      try {
        const data = await fetchAPI<{ users: UserDto[]; isPilot: boolean }>(
          "guide/users",
          {
            method: "GET",
          }
        );
        setUsers(data.users);
        setIsPilot(data.isPilot);
      } catch (err: any) {
        handleApiErrors(err);
        navigate("/account/login");
      } finally {
        setIsLoading(false);
      }
    };

    fetchUsers();
  }, []);

  return (
    <div className="users-container">
      <h1 className="users-title">Globe Users</h1>
      {isLoading ? (
        <div className="spinner-container">
          <Spinner />
        </div>
      ) : (
        <div className="users-grid">
          {users.map((user) => (
            <User key={user.userId} user={user} isPilot={isPilot} />
          ))}
        </div>
      )}
    </div>
  );
};

export default Users;
