import { useNavigate } from "react-router-dom";
import "./Users.scss";
import { useEffect, useState } from "react";
import fetchAPI from "../../shared/utils/fetchAPI";
import { UserDto } from "./models/user";
import { handleApiErrors } from "../../shared/utils/displayApiErrors";
import Spinner from "../../shared/components/Spinner/Spinner";
import Avatar from "../../shared/components/Avatar/Avatar";

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
  const userId = userParsed.id;
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
      <h1>Globe Users</h1>
      {isLoading ? (
        <div className="spinner-container">
          <Spinner />
        </div>
      ) : (
        <div className="users-grid">
          {users.map((user) => (
            <div key={user.userId} className="user-card">
              <div className="user-left">
                <div className="avatar-user-container">
                  <Avatar photoUrl={user.profilePicture} />
                </div>
                <div className="username-and-created-container">
                  <p>{user.username}</p>
                  <p>Created: {new Date(user.createdAt).toLocaleString()}</p>
                </div>
              </div>
              <div className="user-middle">
                {user.isGuide ? (
                  <button
                    type="button"
                    title="edit role button"
                    className="is-guide-button"
                    disabled={!isPilot}
                  >
                    Guide
                  </button>
                ) : (
                  <button
                    type="button"
                    title="edit role button"
                    className="is-traveler-button"
                    disabled={!isPilot}
                  >
                    Traveler
                  </button>
                )}
              </div>
              <div className="user-right">
                <button type="button" title="button" className="delete-button">
                  Message
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Users;
