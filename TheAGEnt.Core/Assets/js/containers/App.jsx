import injectTapEventPlugin from 'react-tap-event-plugin';
injectTapEventPlugin();
import Formsy from 'formsy-react';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import AppBar from 'material-ui/AppBar';
import MenuItem from 'material-ui/MenuItem';
import Drawer from 'material-ui/Drawer';
import FlatButton from 'material-ui/FlatButton';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {Popover, PopoverAnimationVertical} from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import Dialog from 'material-ui/Dialog';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

import FormsyText from 'formsy-material-ui/lib/FormsyText';

const Login_Form = React.createClass({
    getInitialState: function () {
        return {canSubmit: false, email: "", password: ""};
    },

    errorMessages: {
        emailError: "Please type a valid Email",
        passwordError: "Please type a valid Password",
    },
    enableButton() {
        this.setState({
            canSubmit: true,
        });
    },

    disableButton() {
        this.setState({
            canSubmit: false,
        });
    },

    submitForm(data) {
        console.log(data);
        alert(JSON.stringify(data, null, 4));
    },

    notifyFormError(data) {
        console.error('Form error:', data);
    },

    _emailFieldChange: function (e) {
        this.setState({email: e.target.value});
    },
    _passwordFieldChange: function (e) {
        this.setState({password: e.target.value});
    },
    clearForm: function () {
        this.setState({email: "", password: ""});
    },
    sendToServer: function (data) {
        fetch('/token', {
            credentials: 'include',
            method: 'POST',
            headers: new Headers({
                'Accept': 'application/json, application/xml, text/plain, text/html, *.*',
                'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8'
            }),
            body: `grant_type=password&username=${data.email}&password=${data.password}`
        })
            .then(r => {
                if (!r.ok) {
                    alert("Wrong email or password!")
                } else {
                    return r.json()
                }
            })
            .then(data => {
                this.getClaims(data.access_token).then(claims => {
                    if (!claims.includes("banned")) {
                        Cookie.save('userName', data.userName);
                        Cookie.save('tokenInfo', data.access_token);
                        this.props.updateAuthState(true, data.userName);
                        this.props.onRequestClose();
                        this.clearForm();
                    } else {
                        alert("You banned!")
                    }
                });

            });
    },
    getClaims: function (userToken) {
        return new Promise((resolve, reject) => {
            fetch('api/Account/AllUserInfo', {
                method: 'GET',
                headers: new Headers({
                    "Content-Type": "application/json",
                    "Authorization": "bearer " + userToken
                })
            }).then(r => r.json()).then(data => {
                console.log("Claims of user", data.Claims);
                var strOfCookies = data.Claims.reduce((x, y) => x + ";" + y.ClaimValue, "");
                Cookie.save('claims', strOfCookies);
                Cookie.save('nickname', data.NickName);
                resolve(strOfCookies);
            });
        })
    },
    render: function () {
        let {emailError, passwordError} = this.errorMessages;
        return (
            <Dialog title={this.props.title} modal={this.props.modal} open={this.props.open}
                    onRequestClose={this.props.onRequestClose}>
                <div ref="loginForm" className="Login">
                    <Formsy.Form
                        className="Login"
                        onValid={this.enableButton}
                        onInvalid={this.disableButton}
                        onValidSubmit={this.sendToServer}
                        onInvalidSubmit={this.notifyFormError}
                    >
                        <FormsyText
                            className="Login-Input"
                            name="email"
                            validations="isEmail"
                            validationError={emailError}
                            required
                            hintText="Email"
                            floatingLabelText="Enter please You email here"
                            updateImmediately
                        />
                        <FormsyText
                            className="Login-Input"
                            name="password"
                            type="password"
                            validations="minLength:6"
                            validationError={passwordError}
                            required
                            hintText="Password"
                            floatingLabelText="Enter please You password here"
                            updateImmediately
                        />
                        <RaisedButton
                            className="Login-Input"
                            type="submit"
                            label="Ok, let's start!"
                            primary={true}
                            disabled={!this.state.canSubmit}
                        />
                    </Formsy.Form>
                    {/*<TextField name="email" value={this.state.email} onChange={this._emailFieldChange}*/}
                    {/*hintText="Email"*/}
                    {/*floatingLabelText="Enter please You email here" type="email"/><br/>*/}
                    {/*<TextField name="password" value={this.state.password} onChange={this._passwordFieldChange}*/}
                    {/*hintText="Password" floatingLabelText="Enter please You password here"*/}
                    {/*type="password"/><br/>*/}
                    {/*<RaisedButton label="Ok, let's start!" primary={true} onClick={this.sendToServer}/>*/}
                </div>
            </Dialog>
        );
    }
});

module.exports = React.createClass({
    getInitialState: function () {
        return {
            open: false,
            openUserMenu: false,
            openLoginWindow: false,
            LoggedUserName: Cookie.load('userName'),
            logged: false,
            nickName: Cookie.load('nickname') || "",
            claims: Cookie.load('claims') || ""
        };
    },
    handleToggle: function () {
        this.setState({
            open: !this.state.open
        });
    },
    handleClose: function () {
        this.setState({open: false});
    },

    handleTouchTap: function (event) {
        event.preventDefault();
        this.setState({openUserMenu: true, anchorEl: event.currentTarget});
    },
    handleRequestClose: function () {
        this.setState({openUserMenu: false});
    },
    handleOpenLoginWindow: function () {
        this.setState({openLoginWindow: true});
    },
    handleCloseLoginWindow: function () {
        this.setState({openLoginWindow: false});
    },
    updateAuthState: function (flag, userName, nickName) {
        this.setState({logged: flag, LoggedUserName: userName, nickName: nickName});
    },
    updateNickNameState: function (nickname) {
        this.setState({nickName: nickname});
    },
    handleLogOut: function () {
        Cookie.save('userName', "");
        Cookie.save('tokenKey', "");
        Cookie.save('claims', "");
        Cookie.save('nickname', "");
        this.updateAuthState(false, "", "");
    },
    componentDidMount: function () {
        if (this.state.LoggedUserName) {
            this.setState({logged: true});
        } else {
            this.setState({logged: false});
        }
    },
    componentDidUpdate: function (prevProps, prevState) {
        if (prevState.logged !== this.state.logged) {
            if (this.state.LoggedUserName) {
                this.setState({logged: true});
            } else {
                this.setState({logged: false});
            }
        }
    },
    render: function () {
        return (
            <MuiThemeProvider>
                <div className='container'>
                    <AppBar title="AG - You personal art place!" onLeftIconButtonTouchTap={this.handleToggle}
                            iconElementRight={this.state.logged
                                ? < FlatButton icon={ < MoreVertIcon />
                            }
                                               onTouchTap={
                                                   this.handleTouchTap
                                               }/>
                                : <FlatButton onTouchTap={this.handleOpenLoginWindow} label="Log in" primary={true}/>}>
                        <Popover open={this.state.openUserMenu} anchorEl={this.state.anchorEl} anchorOrigin={{
                            horizontal: 'left',
                            vertical: 'bottom'
                        }} targetOrigin={{
                            horizontal: 'left',
                            vertical: 'top'
                        }} onRequestClose={this.handleRequestClose} animation={PopoverAnimationVertical}>
                            <Menu>
                                <MenuItem href="#Settings">Settings</MenuItem>
                                <MenuItem href={`/EditingInfo?email=${this.state.LoggedUserName}`}>Edit info</MenuItem>
                                <MenuItem primaryText="Log out" href="/" onTouchTap={this.handleLogOut}/>
                            </Menu>
                        </Popover>
                    </AppBar>
                    <Login_Form
                        updateNickNameState={this.updateNickNameState}
                        title="Here You'll enter to the amazing world of art!"
                        modal={false}
                        open={this.state.openLoginWindow}
                        onRequestClose={this.handleCloseLoginWindow}
                        updateAuthState={this.updateAuthState}/>
                    <Drawer docked={false} width={200} open={this.state.open}
                            onRequestChange={(open) => this.setState({open})}>
                        <MenuItem href="/" onTouchTap={this.handleClose}>Home</MenuItem>
                        {this.state.claims.includes("admin") ?
                            <MenuItem href="/AdminPanel" onTouchTap={this.handleClose}>Admin Panel</MenuItem> :
                            <div></div>}
                        <MenuItem href="/Registration" onTouchTap={this.handleClose}>Registration</MenuItem>
                        <MenuItem href="/Swagger" onTouchTap={this.handleClose}>Public API</MenuItem>
                    </Drawer>
                    {this.props.children}
                </div>
            </MuiThemeProvider>
        );
    }
});
