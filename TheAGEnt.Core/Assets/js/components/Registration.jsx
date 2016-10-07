import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

let RegistrationForm = React.createClass({
    getInitialState: function() {
        return {
            email: "",
            name: "",
            surname: "",
            nickname: "",
            address: "",
            password: ""
        };
    },
    clearForm: function() {
        this.setState({
            email: "",
            name: "",
            surname: "",
            nickname: "",
            address: "",
            password: "",
            confirmPassword: ""
        });
    },
    _emailFieldChange: function(e) {
        this.setState({email: e.target.value});
    },
    _nameFieldChange: function(e) {
        this.setState({name: e.target.value});
    },
    _surnameFieldChange: function(e) {
        this.setState({surname: e.target.value});
    },
    _nicknameFieldChange: function(e) {
        this.setState({nickname: e.target.value});
    },
    _addressFieldChange: function(e) {
        this.setState({address: e.target.value});
    },
    _passwordFieldChange: function(e) {
        this.setState({password: e.target.value});
    },
    _confirmPasswordFieldChange: function(e) {
        this.setState({confirmPassword: e.target.value});
    },
    sendToServer: function() {
        if (this.state.password === this.state.confirmPassword) {
            var data = {
                email: this.state.email,
                name: this.state.name,
                surname: this.state.surname,
                nickname: this.state.nickname,
                address: this.state.address,
                password: this.state.password,
                confirmPassword: this.state.confirmPassword
            };

        console.log(data);
        fetch('api/Account/Register', {
            method: 'POST',
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load('tokenInfo')
            }),
            body: JSON.stringify(data)
        }).then(() => {
            this.clearForm();
        });
        }
    },
    render: function() {
        return (
            <div className="Registration">
                <TextField value={this.state.email} onChange={this._emailFieldChange} hintText="Email" floatingLabelText="Enter please You email here" type="email"/><br/>
                <TextField value={this.state.name} onChange={this._nameFieldChange} hintText="Name" floatingLabelText="Enter please You name here"/><br/>
                <TextField value={this.state.surname} onChange={this._surnameFieldChange} hintText="Surname" floatingLabelText="Enter please You surname here"/><br/>
                <TextField value={this.state.nickname} onChange={this._nicknameFieldChange} hintText="Nickname" floatingLabelText="Enter please You nickname here"/><br/>
                <TextField value={this.state.address} onChange={this._addressFieldChange} hintText="Adress" floatingLabelText="Enter please You adress here"/><br/>
                <TextField value={this.state.password} onChange={this._passwordFieldChange} hintText="Password" floatingLabelText="Enter please You password here" type="password"/><br/>
                <TextField value={this.state.confirmPassword} onChange={this._confirmPasswordFieldChange} hintText="Confirm Password" floatingLabelText="Re-enter please You password here" type="password"/><br/>
                <RaisedButton label="Ok, let's start!" primary={true} onClick={this.sendToServer}/>
            </div>
        );
    }
});

module.exports = React.createClass({
    getInitialState: function() {
        return {foo: ""};
    },
    render: function() {
        return (
            <div className="Registration">
                <RegistrationForm/>
            </div>
        );
    }
});
