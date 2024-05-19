import { toast } from "react-toastify";

const displayedMessages = new Set();

export const handleApiErrors = (error: any) => {
  console.log(error);

  const showToast = (message: string) => {
    if (!displayedMessages.has(message)) {
      toast.error(message, {
        position: "bottom-right",
        bodyClassName: "custom-body",
        className: "custom-toast",
        autoClose: 3000,
        hideProgressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        onClose: () => displayedMessages.delete(message),
      });
      displayedMessages.add(message);
    }
  };

  if (error.errors) {
    Object.keys(error.errors).forEach((key) => {
      error.errors[key].forEach((message: string) => {
        showToast(`${key}: ${message}`);
      });
    });
  } else if (error.status === "401") {
    showToast("You are not authorized to perform this action");
  } else if (error.message) {
    showToast(error.message);
  } else {
    showToast("An unknown error occurred");
  }
};
