import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';

module.exports = React.createClass({
    getInitialState: function() {
        return {email: this.props.email, name: this.props.name, surname: this.props.surname, nickname: this.props.nickname, address: this.props.address};
    },
    sendToServer: function() {
        var data = {
            Email: this.state.email,
            Name: this.state.name,
            Surname: this.state.surname,
            Nickname: this.state.nickname,
            Address: this.state.address
        };

        fetch('api/Account/UpdateUserInfo', {
            method: 'POST',
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load('tokenInfo')
            }),
            body: JSON.stringify(data)
        }).then(() => {
          this.props.changingUserInfo(data);
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
    render: function() {
        return (
            <div className="Edit">
                <h3>Change You info here!</h3>
                <TextField value={this.state.email} onChange={this._emailFieldChange} hintText={this.props.email} floatingLabelText="Enter please You email here" type="email"/><br/>
                <TextField value={this.state.name} onChange={this._nameFieldChange} hintText={this.props.name} floatingLabelText="Enter please You name here"/><br/>
                <TextField value={this.state.surname} onChange={this._surnameFieldChange} hintText={this.props.surname} floatingLabelText="Enter please You surname here"/><br/>
                <TextField value={this.state.nickname} onChange={this._nicknameFieldChange} hintText={this.props.nickname} floatingLabelText="Enter please You nickname here"/><br/>
                <TextField value={this.state.address} onChange={this._addressFieldChange} hintText={this.props.address} floatingLabelText="Enter please You adress here"/><br/>
                <RaisedButton label="Ok, save my new info!" primary={true} onClick={this.sendToServer}/>
            </div>
        );
    }
});
