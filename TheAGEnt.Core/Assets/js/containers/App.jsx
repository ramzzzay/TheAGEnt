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
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

const Login_Form = React.createClass({
    getInitialState: function() {
        return {email: "", password: ""};
    },
    _emailFieldChange: function(e) {
        this.setState({email: e.target.value});
    },
    _passwordFieldChange: function(e) {
        this.setState({password: e.target.value});
    },
    clearForm: function() {
        this.setState({email: "", password: ""});
    },
    sendToServer: function() {
        fetch('/token', {
            credentials: 'include',
            method: 'POST',
            headers: new Headers({'Accept': 'application/json, application/xml, text/plain, text/html, *.*', 'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8'}),
            body: `grant_type=password&username=${this.state.email}&password=${this.state.password}`
        }).then(r => r.json()).then(data => {
            Cookie.save('userName', data.userName);
            Cookie.save('tokenInfo', data.access_token);
            this.getClaims();
            this.props.updateAuthState(true, data.userName);
            this.props.onRequestClose();
            this.clearForm();
        });
    },
    getClaims: function() {
        fetch('api/Account/AllUserInfo', {
            method: 'GET',
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load('tokenInfo')
            })
        }).then(r => r.json()).then(data => {
            var strOfCookies = data.Claims.reduce((x, y) => x + ";" + y.ClaimValue, "");
            Cookie.save('claims', strOfCookies);
            Cookie.save('nickname', data.NickName);
        });
    },
    render: function() {
        return (
            <Dialog title={this.props.title} modal={this.props.modal} open={this.props.open} onRequestClose={this.props.onRequestClose}>
                <div ref="loginForm" className="Login">
                    <TextField name="email" value={this.state.email} onChange={this._emailFieldChange} hintText="Email" floatingLabelText="Enter please You email here" type="email"/><br/>
                    <TextField name="password" value={this.state.password} onChange={this._passwordFieldChange} hintText="Password" floatingLabelText="Enter please You password here" type="password"/><br/>
                    <RaisedButton label="Ok, let's start!" primary={true} onClick={this.sendToServer}/>
                </div>
            </Dialog>
        );
    }
});

module.exports = React.createClass({
    getInitialState: function() {
        return {open: false, openUserMenu: false, openLoginWindow: false, LoggedUserName: Cookie.load('userName'), logged: false, nickName:""};
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
    updateAuthState: function(flag, userName) {
        this.setState({logged: flag, LoggedUserName: userName});
    },
    updateNickNameState:function(nickname){
      this.setState({nickName:nickname});
    },
    handleLogOut: function() {
        Cookie.save('userName', "");
        Cookie.save('tokenKey', "");
        Cookie.save('claims', "");
        this.updateAuthState(false, "");
    },
    componentDidMount: function() {
        if (typeof this.state.LoggedUserName !== "undefined" && this.state.LoggedUserName !== "") {
            this.setState({logged: true});
        } else {
            this.setState({logged: false});
        }
    },
    componentDidUpdate: function(prevProps, prevState) {
        if (prevState.logged !== this.state.logged) {
            if (typeof this.state.LoggedUserName !== "undefined" && this.state.LoggedUserName !== "") {
                this.setState({logged: true});
            } else {
                this.setState({logged: false});
            }
        }
    },
    render: function() {
        return (
            <MuiThemeProvider>
                <div className='container'>
                    <AppBar title="AG - You personal art place!" onLeftIconButtonTouchTap={this.handleToggle} iconElementRight={this.state.logged
                        ? < FlatButton icon = { < MoreVertIcon />
                        }
                        onTouchTap = {
                            this.handleTouchTap
                        } />
                        : <FlatButton onTouchTap={this.handleOpenLoginWindow} label="Log in" primary={true}/>}>
                        <Popover open={this.state.openUserMenu} anchorEl={this.state.anchorEl} anchorOrigin={{
                            horizontal: 'left',
                            vertical: 'bottom'
                        }} targetOrigin={{
                            horizontal: 'left',
                            vertical: 'top'
                        }} onRequestClose={this.handleRequestClose} animation={PopoverAnimationVertical}>
                            <Menu>
                                <MenuItem href="/Settings">Settings</MenuItem>
                                <MenuItem href={`EditingInfo?email=${this.state.nickName}`}>Edit info</MenuItem>
                                <MenuItem primaryText="Log out" onTouchTap={this.handleLogOut}/>
                            </Menu>
                        </Popover>
                    </AppBar>
                    <Login_Form updateNickNameState={this.updateNickNameState} title="Here You'll enter to the amazing world of art!" modal={false} open={this.state.openLoginWindow} onRequestClose={this.handleCloseLoginWindow} updateAuthState={this.updateAuthState}/>
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
