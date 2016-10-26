import FlatButton from 'material-ui/FlatButton';
import RaisedButton from 'material-ui/RaisedButton';
import ContentAdd from 'material-ui/svg-icons/content/add';

const styles = {
    exampleImageInput: {
        cursor: 'pointer',
        position: 'absolute',
        top: 0,
        bottom: 0,
        right: 0,
        left: 0,
        width: '100%',
        opacity: 0,
    },
    preview: {
        display: "block",
        height: "250px",
        width: "250px",
        marginBottom: "25px"
    }
};

module.exports = React.createClass({
    getInitialState: function () {
        return {
            file: "",
            imagePreviewUrl: ""
        };
    },
    gotFile: function (e) {
        console.log(e.target.files);
        var file = e.target.files[0];
        var reader = new FileReader();
        reader.onloadend = () => {
            this.setState({
                file: file,
                imagePreviewUrl: reader.result
            });
        };
        reader.readAsDataURL(file);
    },
    send: function () {
        var formData = new FormData();
        formData.append("file",this.state.file);

        this.props.email && formData.append("Email", this.props.email);
        this.props.albumName && formData.append("Album", this.props.albumName);

        fetch(this.props.url, {
            method: "POST",
            headers: new Headers({
                "Authorization": "bearer " + Cookie.load("tokenInfo"),
            }),
            body: formData
        })
            .then(r => r.json())
            .then(data=> this.props.changeState && this.props.changeState(data.uploadedUrl));
    },
    render: function () {
        return (
            <div className="Image-Uploader">
                <h3>Uploading {this.props.type}</h3>
                <FlatButton onChange={this.gotFile} label="Choose an Image" labelPosition="before" icon={<ContentAdd />}>
                    <input type="file" style={styles.exampleImageInput}/>
                </FlatButton>
                <img style={styles.preview} src={this.state.imagePreviewUrl}/>
                <RaisedButton onClick={this.send} label="Upload!" primary={true} />
            </div>
        );
    }
});
﻿
