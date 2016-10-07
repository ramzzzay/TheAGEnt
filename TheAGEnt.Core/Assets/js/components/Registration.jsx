import TextField from 'material-ui/TextField';

module.exports = React.createClass({
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
    render: function() {
        return (
            <div className="Registration">
                <TextField hintText="Email" floatingLabelText="Enter please You email here"/><br/>
                <TextField hintText="Name" floatingLabelText="Enter please You name here"/><br/>
                <TextField hintText="Surname" floatingLabelText="Enter please You surname here"/><br/>
                <TextField hintText="Nickname" floatingLabelText="Enter please You nickname here"/><br/>
                <TextField hintText="Adress" floatingLabelText="Enter please You adress here"/><br/>
                <TextField hintText="Password" floatingLabelText="Enter please You password here" type="password"/><br/>
                <TextField hintText="Password" floatingLabelText="Re-enter please You password here" type="password"/><br/>
            </div>
        );
    }
});
