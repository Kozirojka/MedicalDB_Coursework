import { useState } from "react";
import { useAuth } from "../../contexts/AuthContext";
import "../../styles/Admin/AdminDashboard.css";
import RequestsTab from "../../components/Admin/featureTabs/RequestsTab";
import RegisterDoctorType from "../../components/Admin/featureTabs/RegisterDoctorType";
import UsersTab from "../../components/Admin/featureTabs/UsersTab";
import Sidebar from "../../components/Shared/Sidebar/Sidebar";
import ListAltIcon from "@mui/icons-material/ListAlt";
import PeopleIcon from "@mui/icons-material/People";
import PersonIcon from "@mui/icons-material/Person";
import MedicalInformationIcon from '@mui/icons-material/MedicalInformation';
export default function AdminDashboard() {
  const { logout } = useAuth();
  const [activeTab, setActiveTab] = useState("requests");

  const tabsConfig = [
    {
      key: "requests",
      label: "Очікувані запити",
      icon: <ListAltIcon />,
      component: <RequestsTab />,
    },
    {
      key: "users",
      label: "Користувачі",
      icon: <PeopleIcon />,
      component: <UsersTab />,
    },
    {
      key: "register",
      label: "Реєстрація лікаря",
      icon: <MedicalInformationIcon />,
      component: <RegisterDoctorType />,
    }
  ];

  const handleLogout = () => {
    logout();
  };

  return (
    <div className="dashboard-container">
      <Sidebar
        tabsConfig={tabsConfig}
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        onLogout={handleLogout}
      />

      <div className="main-content">
        {tabsConfig.map((tabItem) => {
          if (tabItem.key === activeTab) {
            return <div key={tabItem.key}>{tabItem.component}</div>;
          }
          return null;
        })}
      </div>
    </div>
  );
}
