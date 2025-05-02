import React from "react";
import { 
  List, 
  ListItem, 
  ListItemButton, 
  ListItemIcon, 
  Drawer, 
  Divider, 
  Box,
  IconButton,
  Tooltip
} from "@mui/material";
import ExitToAppIcon from '@mui/icons-material/ExitToApp';

const drawerWidth = 80;

const Sidebar = ({ tabsConfig, activeTab, setActiveTab, onLogout }) => {
  return (
    <Drawer
      variant="permanent"
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
          bgcolor: 'background.paper',
          borderRight: '1px solid rgba(0, 0, 0, 0.12)',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'space-between',
          padding: '8px 0'
        },
      }}
    >
      <List disablePadding>
        {tabsConfig.map((tab) => (
          <ListItem key={tab.key} disablePadding sx={{ display: 'block', mb: 1 }}>
            <Tooltip title={tab.label} placement="right">
              <ListItemButton
                onClick={() => setActiveTab(tab.key)}
                selected={activeTab === tab.key}
                sx={{
                  minHeight: 48,
                  justifyContent: 'center',
                  px: 2.5,
                  borderRadius: '8px',
                  mx: 0.5,
                  '&.Mui-selected': {
                    bgcolor: 'primary.light',
                    '&:hover': {
                      bgcolor: 'primary.light',
                    },
                    '& .MuiListItemIcon-root': {
                      color: 'primary.main',
                    }
                  }
                }}
              >
                <ListItemIcon
                  sx={{
                    minWidth: 0,
                    mr: 0,
                    justifyContent: 'center',
                  }}
                >
                  {tab.icon}
                </ListItemIcon>
              </ListItemButton>
            </Tooltip>
          </ListItem>
        ))}
      </List>

      <Box>
        <Divider />
        <Box sx={{ p: 1, display: 'flex', justifyContent: 'center' }}>
          <Tooltip title="Вийти" placement="right">
            <IconButton onClick={onLogout} color="primary">
              <ExitToAppIcon />
            </IconButton>
          </Tooltip>
        </Box>
      </Box>
    </Drawer>
  );
};

export default Sidebar;