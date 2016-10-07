import {Link} from 'react-router'
import injectTapEventPlugin from 'react-tap-event-plugin';
injectTapEventPlugin();

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';

import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import IconMenu from 'material-ui/IconMenu';
import MenuItem from 'material-ui/MenuItem';

import Drawer from 'material-ui/Drawer';

import FlatButton from 'material-ui/FlatButton';

module.exports = React.createClass({
    getInitialState: function() {
        return {open: false};
    },
    handleToggle: function() {
        this.setState({
            open: !this.state.open
        });
    },
    handleClose: () => this.setState({open: false}),
    render: function() {
        return (
            <MuiThemeProvider>
                <div className='container'>
                    <AppBar title="AG" iconClassNameRight="muidocs-icon-navigation-expand-more" onLeftIconButtonTouchTap={this.handleToggle}/>
                    <Drawer docked={false} width={200} open={this.state.open} onRequestChange={(open) => this.setState({open})}>
                        <MenuItem href="/" onTouchTap={this.handleClose}>Home</MenuItem>
                        <MenuItem href="/AdminPanel" onTouchTap={this.handleClose}>Admin Panel</MenuItem>
                        <MenuItem href="/Registration" onTouchTap={this.handleClose}>Registration</MenuItem>
                        <MenuItem href="/Swagger" onTouchTap={this.handleClose}>Public API</MenuItem>
                    </Drawer>
                    {this.props.children}
                </div>
            </MuiThemeProvider>
        );
    }
});
