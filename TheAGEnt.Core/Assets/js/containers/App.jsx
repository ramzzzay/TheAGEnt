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
import FontIcon from 'material-ui/FontIcon';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {fullWhite} from 'material-ui/styles/colors';
import {Popover, PopoverAnimationVertical} from 'material-ui/Popover';
import Menu from 'material-ui/Menu';

import Dialog from 'material-ui/Dialog';


// TODO: Why this not work? I must fix that!
let loginForm = React.createClass({
    getInitialState: function() {
        return {email: "", password: ""};
    },
    _emailFieldChange: function(e) {
        this.setState({email: e.target.value});
    },
    render: function(){
      return (
        <div ref="loginForm">
            <TextField value={this.state.email} onChange={this._emailFieldChange} hintText="Email" floatingLabelText="Enter please You email here" type="email"/><br/>
        </div>
      );
    }
});

module.exports = React.createClass({
    getInitialState: function() {
        return {open: false, openUserMenu: false, openLoginWindow: false};
    },
    handleToggle: function() {
        this.setState({
            open: !this.state.open
        });
    },
    handleClose: function() {
        this.setState({open: false});
    },

    handleTouchTap: function(event) {
        event.preventDefault();
        this.setState({openUserMenu: true, anchorEl: event.currentTarget});
    },
    handleRequestClose: function() {
        this.setState({openUserMenu: false});
    },

    handleOpenLoginWindow: function() {
        this.setState({openLoginWindow: true});
    },

    handleCloseLoginWindow: function() {
        this.setState({openLoginWindow: false});
    },
    render: function() {
        var actions = [ < FlatButton label = "Cancel" primary = {
                true
            }
            onTouchTap = {
                this.handleCloseLoginWindow
            } />, < FlatButton label = "Submit" primary = {
                true
            }
            keyboardFocused = {
                true
            }
            onTouchTap = {
                this.handleCloseLoginWindow
            } />
        ];
        return (
            <MuiThemeProvider>
                <div className='container'>
                    <AppBar title="AG - You personal art place!" iconClassNameRight="muidocs-icon-navigation-expand-more" onLeftIconButtonTouchTap={this.handleToggle} iconElementRight={< FlatButton icon = { < MoreVertIcon />
                    }
                    onTouchTap = {
                        this.handleTouchTap
                    } />}>
                        <Popover open={this.state.openUserMenu} anchorEl={this.state.anchorEl} anchorOrigin={{
                            horizontal: 'left',
                            vertical: 'bottom'
                        }} targetOrigin={{
                            horizontal: 'left',
                            vertical: 'top'
                        }} onRequestClose={this.handleRequestClose} animation={PopoverAnimationVertical}>
                            <Menu>
                                <MenuItem onTouchTap={this.handleOpenLoginWindow} primaryText="Log in"/>
                                <MenuItem primaryText="Settings"/>
                                <MenuItem primaryText="Sign out"/>
                            </Menu>
                        </Popover>
                    </AppBar>
                    <Dialog title="Dialog With Actions" actions={actions} modal={false} open={this.state.openLoginWindow} onRequestClose={this.handleCloseLoginWindow}>
                        <loginForm/>
                    </Dialog>
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
